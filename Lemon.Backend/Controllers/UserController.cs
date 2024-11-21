using Lemon.Backend.Entities;
using Lemon.Backend.Models.User;
using Lemon.Backend.Services;
using Lemon.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lemon.Backend.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IRepositoryWrapper _repository;

    public UserController(ILogger<UserController> logger, IRepositoryWrapper repositoryWrapper)
    {
        _logger = logger;
        _repository = repositoryWrapper;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="userRegisterDto">注册信息</param>
    /// <returns>注册结果</returns>
    [HttpPost("register", Name = "Register")]
    [ProducesResponseType(typeof(UserRegisterResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserRegisterResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var userRepository = _repository.UserRepository;

        var (passwordHash, passwordSalt) = Utils.Password.CreatePasswordHash(userRegisterDto.Password);
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            UserName = id.ToString(),
            Email = userRegisterDto.Email,
            PhoneNumber = userRegisterDto.PhoneNumber,
            NickName = $"user_{id.ToString()[..8]}",
            PasswordHash = passwordHash,
            Salt = passwordSalt,
        };

        var result = await userRepository.AddAsync(user);
        if (!result)
        {
            _logger.LogWarning("User {} register failed", user.Id);
            return StatusCode
            (
                StatusCodes.Status500InternalServerError,
                UserRegisterResponseDto.Fail("Register Failed")
            );
        }

        _logger.LogDebug("User {} registered", user.Id);
        return Ok(UserRegisterResponseDto.Success("Register Success"));
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="userLoginDto">登录信息</param>
    /// <returns>登录结果</returns>
    /// <response code="200">登录成功</response>
    /// <response code="400">登录失败</response>
    [HttpPost("login", Name = "Login")]
    [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        if (!userLoginDto.IsValid())
        {
            return StatusCode
            (
                StatusCodes.Status400BadRequest,
                UserLoginResponseDto.Fail("Invalid Input")
            );
        }

        var userRepository = _repository.UserRepository;
        User? user;
        if (userLoginDto.Email != null)
        {
            user = await userRepository.GetByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                return StatusCode
                (
                    StatusCodes.Status400BadRequest,
                    UserLoginResponseDto.Fail("User Not Found")
                );
            }
        }
        else if (userLoginDto.PhoneNumber != null)
        {
            user = await userRepository.GetByPhoneNumberAsync(userLoginDto.PhoneNumber);
            if (user == null)
            {
                return StatusCode
                (
                    StatusCodes.Status400BadRequest,
                    UserLoginResponseDto.Fail("User Not Found")
                );
            }
        }
        else if (userLoginDto.UserName != null)
        {
            user = await userRepository.GetByUserNameAsync(userLoginDto.UserName);
            if (user == null)
            {
                return StatusCode
                (
                    StatusCodes.Status400BadRequest,
                    UserLoginResponseDto.Fail("User Not Found")
                );
            }
        }
        else
        {
            return StatusCode
            (
                StatusCodes.Status400BadRequest,
                UserLoginResponseDto.Fail("Invalid Input")
            );
        }

        if (!Utils.Password.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.Salt))
        {
            return StatusCode
            (
                StatusCodes.Status400BadRequest,
                UserLoginResponseDto.Fail("Password Error")
            );
        }

        if (!Token.Store.TokenStore.TryGenerateAndAddToken(user.Id, user.NickName, DateTime.Now.AddHours(Constants.JWTToken.TokenExpireTime), out var token))
        {
            return StatusCode
            (
                StatusCodes.Status500InternalServerError,
                UserLoginResponseDto.Fail("Generate Token Failed")
            );
        }

        return Ok(UserLoginResponseDto.Success(token));
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户信息</returns>
    [HttpGet("{id}/profile", Name = "Profile")]
    [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfile([FromRoute] Guid id)
    {
        var userRepository = _repository.UserRepository;
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return StatusCode
            (
                StatusCodes.Status400BadRequest,
                UserProfileResponseDto.Fail("User Not Found")
            );
        }

        return Ok(UserProfileResponseDto.Success(user));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("auth-test", Name = "TestAuth")]
    public IActionResult ProtectedEndpoint()
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
        {
            return Unauthorized("User ID not found.");
        }

        return Ok($"Authenticated User ID: {userId}");
    }
}
