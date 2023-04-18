# Projeto de Login Completo em .NET 6

Este é um projeto completo de login desenvolvido em .NET 6, com as seguintes funcionalidades:

- Cadastro de usuários
- Recuperação de senha
- Login com autenticação em dois fatores via e-mail
- Autenticação via JWT
- Front-end desenvolvido em MVC
- Back-end utilizando o padrão DDD

Utilização das seguintes bibliotecas:

- `Identity` para criptografia das senhas
- `Entity Framework` para o repositório de dados
- `Bootstrap` para estilização do front-end
- `AspNetCore.Authentication.Cookies` para gerenciamento de cookies
- `Sweetalert2` para exibição de mensagens de alerta
- `AutoMapper` para mapeamento de objetos
- `SQL Server` como banco de dados

## Tecnologias Utilizadas

- .NET 6
- C#
- Entity Framework
- Identity
- MVC
- Bootstrap
- AspNetCore.Authentication.Cookies
- Sweetalert2
- AutoMapper
- SQL Server

## Funcionalidades

### Cadastro de Usuários

A funcionalidade de cadastro de usuários permite que novos usuários se registrem no sistema informando nome, e-mail e senha. A senha é criptografada utilizando o `Identity`.

### Recuperação de Senha

Caso um usuário esqueça sua senha, ele poderá utilizar a funcionalidade de recuperação de senha informando seu e-mail. O sistema enviará um e-mail para o usuário com um link para redefinir sua senha.

### Login com Autenticação em Dois Fatores via E-mail

Após o usuário realizar o login, o sistema enviará um código de verificação para o e-mail cadastrado. O usuário deverá informar esse código para acessar o sistema.

### Autenticação via JWT

O sistema utiliza autenticação via `JWT` (JSON Web Token) para manter o usuário autenticado durante a sessão.

### Front-end desenvolvido em MVC

O front-end do sistema foi desenvolvido utilizando o padrão `MVC` (Model-View-Controller), utilizando a linguagem `Razor` para a criação das páginas.

### Back-end utilizando o padrão DDD

O back-end do sistema foi desenvolvido utilizando o padrão `DDD` (Domain-Driven Design), que separa as responsabilidades em diferentes camadas.

## Bibliotecas Utilizadas

- `Microsoft.AspNetCore.Identity`
- `Microsoft.EntityFrameworkCore`
- `Bootstrap`
- `Sweetalert2`
- `AutoMapper`
- `Microsoft.AspNetCore.Authentication.Cookies`

## Conclusão

Este projeto de login completo em .NET 6 oferece diversas funcionalidades de segurança e privacidade para os usuários, além de utilizar as tecnologias mais modernas e eficientes para desenvolvimento web.
