
namespace _4Tech._4Manager.Application.Common.Strings
{
    public static class Messages 
    {
        public static class User
        {
            public const string Deleted = "Usuário deletado com sucesso.";
            public const string Found = "Usuários encontrados com sucesso.";
            public const string LoginSuccess = "Login realizado com sucesso.";
            public const string SignUpSuccess = "Cadastro realizado com sucesso.";
            public const string ProfileUpdated = "Perfil atualizado com sucesso.";
            public const string PhotoUpdated = "Foto de perfil atualizada com sucesso.";
            public const string UserNotAuthenticated = "Usuário não autenticado";
            public const string EmailResetPassword = "E-mail de redefinição de senha enviado com sucesso.";
            public const string ResetPassword = "A senha do usuário foi trocada com sucesso!";
            public const string IncorrectPassword = "A senha atual informada está incorreta.";
        }

        public static class Project
        {
            public const string Created = "Projeto criado com sucesso.";
            public const string Updated = "Projeto atualizado com sucesso.";
            public const string Deleted = "Projeto deletado com sucesso.";
            public const string ProjectNameExists = "Já existe um projeto com esse nome.";
        }

        public static class Timesheet
        {
            public const string Returned = "Timesheets retornados com sucesso.";
            public const string Created = "Timesheet criado com sucesso.";
            public const string Updated = "Timesheet atualizado com sucesso.";
            public const string Deleted = "Timesheet deletado com sucesso.";
            public const string TimerInit = "Timer iniciado com sucesso.";
        }

        public static class Auth
        {
            public const string ErrorResetPassword = "Erro ao atualizar senha";
            public const string UnauthorizedView = "Você não tem permissão para acessar este recurso.";
            public const string UnauthorizedAction = "Você não tem permissão para executar esta operação.";
        }
    }

}
