using Api.Models.Auth;
using Application.Repository;
using Domain.DataBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(SignInManager<User> signInManager, IUserRepository usersRepository) : ControllerBase
{
    [HttpPost]
    [Route("Login")]
    [Produces(MediaTypeNames.Application.FormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> Login(
        [FromForm] string username,
        [FromForm] string password,
        [FromForm] string? twoFactorCode,
        [FromForm] string? twoFactorRecoveryCode,
        [FromQuery] bool useCookies,
        [FromQuery] bool useSessionCookies
    )
    {
        var user = await usersRepository.GetByLogin(username);
        if (user != null && user.Blocked)
        {
            var checkPasswordResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (checkPasswordResult.Succeeded && user.Blocked)
                return TypedResults.Problem("Доступ запрещен", statusCode: StatusCodes.Status403Forbidden);
        }

        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme ?
            IdentityConstants.ApplicationScheme :
            IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(
            username,
            password,
            isPersistent,
            lockoutOnFailure: false
        );

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(twoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(twoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(twoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(twoFactorRecoveryCode);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Problem("Не верный логин или пароль", statusCode: StatusCodes.Status401Unauthorized);
        }

        return TypedResults.Empty;
    }

    [Authorize]
    [HttpPost]
    [Route("Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> Logout()
    {
        // TODO: Revoke token
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    [Authorize]
    [HttpGet]
    [Route("UserInfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> UserInfo()
    {
        var claimsPrincipal = signInManager.Context.User;
        var userManager = signInManager.UserManager;
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
        {
            return TypedResults.NotFound();
        }

        var userInfo = new UserInfo()
        {
            Name = user.Name!,
            Role = "editor"
        };

        return TypedResults.Ok(userInfo);
    }
}
