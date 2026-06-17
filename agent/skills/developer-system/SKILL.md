---
name: developer-system
description: Diretrizes e padrões de desenvolvimento C# com foco em Clean Code, Clean Architecture, SQL e tecnologias modernas do .Net.
---

# Guia de Desenvolvimento C# - Developer System

Este documento define os padrões técnicos, de arquitetura e boas práticas de codificação que devem ser seguidos ao desenvolver código em C# e .NET neste repositório. O objetivo é garantir um código legível, testável, sustentável e de alta performance.

---

## 1. Princípios de Clean Code em C#

*   **Nomes Significativos:** Classes devem ter nomes de substantivos (`ClienteService`, `SolicitacaoRepository`). Métodos devem conter verbos (`ProcessarPagamento`, `ObterPorId`).
*   **Métodos Pequenos e Focados:** Cada método deve realizar apenas uma tarefa (Single Responsibility Principle) e possuir, preferencialmente, menos de 20 linhas.
*   **Evite Comentários Óbvios:** O código deve ser autoexplicativo. Use comentários apenas para explicar decisões de design complexas ou regras de negócio confusas ("o porquê", não "o como").
*   **Tratamento de Erros Eficiente:** Evite blocos `try-catch` gigantes que escondem exceções. Lance exceções específicas e trate-as em middlewares de nível global (como em ASP.NET Core) sempre que possível.
*   **Uso de Recursos com `using`:** Sempre utilize a declaração `using` simplificada para descartar objetos que implementam `IDisposable` (ex: conexões com banco de dados, streams).
    ```csharp
    using var connection = new SqlConnection(connectionString);
    ```

---

## 2. Clean Architecture (Arquitetura Limpa)

O projeto deve ser estruturado em camadas para separar a lógica de negócio dos detalhes de infraestrutura e entrega:

1.  **Domain (Domínio):** O núcleo da aplicação. Contém as Entidades, Objetos de Valor (Value Objects), Interfaces (Contratos) e Regras de Negócio fundamentais. Não possui dependências externas.
2.  **Application (Aplicação):** Orquestra o fluxo de dados. Contém Casos de Uso (Use Cases), DTOs (Data Transfer Objects), Mapeadores e validações. Depende apenas da camada de Domínio.
3.  **Infra.Data (Infraestrutura/Dados):** Implementa os repositórios, contexto do Entity Framework Core, migrações e acessos diretos ao banco de dados SQL.
4.  **Presentation / Site (Apresentação):** A interface com o usuário (Web APIs, Blazor, MVC). Responsável por receber requisições e devolver respostas.

---

## 3. Banco de Dados SQL e Acesso a Dados

*   **Entity Framework Core (EF Core):**
    *   **Queries AsNoTracking:** Use `.AsNoTracking()` para consultas de apenas leitura, melhorando a performance e reduzindo consumo de memória.
    *   **Mapeamento Explícito (Fluent API):** Não utilize anotações de dados (Data Annotations) nas entidades de domínio para configurar tabelas. Mantenha as configurações na camada de Infraestrutura usando `IEntityTypeConfiguration<T>`.
    *   **Consultas Eficientes:** Evite trazer dados desnecessários. Projete apenas as propriedades requeridas usando `.Select()`.
*   **SQL Puro e Dapper:**
    *   Para consultas extremamente complexas ou críticas em termos de performance, utilize o **Dapper** ou queries raw do SQL de forma parametrizada para evitar vulnerabilidades de *SQL Injection*.
*   **Transações:** Agrupe operações de escrita correlacionadas sob uma mesma transação para garantir atomicidade.

---

## 4. Tecnologias e Recursos Modernos do .NET (.NET 8+)

Aproveite os recursos mais recentes da linguagem C# e da plataforma .NET:

*   **Records:** Utilize `record` para estruturas de dados imutáveis (como DTOs ou Commands/Queries).
    ```csharp
    public record ClienteDto(int Id, string Nome, string Email);
    ```
*   **Pattern Matching:** Escreva condicionais mais limpas e expressivas.
    ```csharp
    var mensagem = status switch
    {
        StatusSolicitacao.Pendente => "Aguardando aprovação",
        StatusSolicitacao.Aprovado => "Pronto para processamento",
        _ => "Status desconhecido"
    };
    ```
*   **Minimal APIs:** Para novas rotas e microsserviços, prefira o modelo de Minimal APIs em vez de controllers tradicionais para reduzir overhead e boilerplate.
*   **Injeção de Dependência Nativa (DI):** Utilize lifetimes adequados ao registrar serviços no `IServiceCollection`:
    *   `Transient`: Para serviços leves e sem estado.
    *   `Scoped`: Para serviços que mantêm estado durante uma requisição (como DbContext).
    *   `Singleton`: Criado uma única vez para toda a aplicação.
*   **Expressões Lambda e LINQ:** Utilize a sintaxe de método do LINQ para consultas fluídas e expressivas.

---

## 5. Testabilidade e Qualidade

*   **Testes Unitários:** Escreva testes unitários focados na lógica de negócio (camada de domínio e aplicação). Use frameworks como *xUnit*, *NSubstitute* (para mocks) e *FluentAssertions* (para asserções legíveis).
*   **Inversão de Controle:** Programe sempre voltado para interfaces, permitindo a fácil substituição e mocking de componentes em ambiente de testes.
