
# Projeto de marcação de tempo de trabalho

O projeto é uma POC em .NET de um domínio de marcação de tempo de trabalho de projetos por usuários. 

Construído com boas práticas de arquitetura e clean code, além de testes unitários cobrindo a aplicação.

A autenticação é feita via JWT e os dados são persistidos em um banco local SQLite na pasta da aplicação.

# Observações

Ao iniciar a aplicação é criado um usuário admin para ser utilizado na autenticação e criação de demais usuários.

Usuário: admin
Senha: admin123

# Design patterns utilizados

Na construção do projeto foram usados os seguintes design patterns:

- Aggregate (representação em hierariquia das entidades)
- CQRS (separação entre requests de busca e alterações no domínio)
- Repository (abstração da camada de acesso a dados)
- Domain Services (abstração de regras de negócio em serviços)
- UnitOfWork (abstração da persistência dos dados)

# Bibliotecas utilizadas

- FluentValidation (validação das entidades e requests)
- EntityFrameworkCore (ORM de peristência de dados)
- Bogus (criação de valores fake para os testes unitários)
- FluentAssertions (asserção dos testes unitários)
- Moq (mock de interfaces)


