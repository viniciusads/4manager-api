using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Supabase;
using Supabase.Gotrue;
using _4Manager.Application.Features.Users.Commands;
using _4Manager.Application.Features.Users.Dtos;
using _4Manager.Application.Features.Users.Handlers;
using _4Manager.Domain.Entities;
using _4Manager.Domain.Enums;
using _4Manager.Application.Interfaces;

using SupabaseUser = Supabase.Gotrue.User;
using DomainUser = _4Manager.Domain.Entities.User;


namespace _4Manager.Application.Tests.Features.Users.Handlers
{
    public class SignUpUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IAuthService> _authServiceMock = new();

        [Fact]
        public async Task ValidCommand_ReturnsUserResponseDto()
        {
            var command = new SignUpUserCommand("João", "joao@email.com", "123456", "123456");

            var supabaseUser = new SupabaseUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = command.Email,
            };

            var session = new Session
            {
                User = supabaseUser,
                AccessToken = "fake-access-token",
                RefreshToken = "fake-refresh-token"
            };

            _authServiceMock.Setup(x => x.SignUpAsync(command.Email, command.Password))
                .ReturnsAsync(session);

            _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<DomainUser>()))
                .Returns(Task.CompletedTask)
                .Callback<DomainUser>(user =>
                {
                    
                    Assert.Equal(command.Name, user.Name);
                    Assert.Equal(command.Email, user.Email);
                });

            var handler = new SignUpUserCommandHandler(_authServiceMock.Object, _userRepositoryMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Email, result.Email);
            Assert.True(result.isActive);
            Assert.Equal(RoleEnum.Analista.ToString(), result.Role);
        }

        [Fact]
        public async Task PasswordsDoNotMatch_ThrowsException()
        {
            var command = new SignUpUserCommand("João", "joao@email.com", "123456", "654321");
            var handler = new SignUpUserCommandHandler(_authServiceMock.Object, _userRepositoryMock.Object);

            var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("As senhas não coincidem.", ex.Message);
        }
    }
}
