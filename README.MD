#API de Cadastro de Produtos e Categorias utilizando JWT , Com Autenticação e Autorização
#
#Pacotes Utiliados do nuget
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Authentication
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.5
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.5
dotnet add package Swashbuckle.AspNetCore --version 5.5.0

#Comandos do Migrations do EF Core para Criar as tabelas no Banco SQL Server
dotnet ef migrations add InitialCreate
dotnet ef database update