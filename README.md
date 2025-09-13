## Projeto CRUD de Usuário: Um Guia Detalhado
Link do desafio: [Link PRIVADO](https://drive.google.com/file/d/1ebhn52BzlPTXM2EndGbx0PhWrwvhj3dp/view?usp=drive_link)

Este projeto implementa um **CRUD (Create, Read, Update, Delete)** para a tabela de customer, seguindo uma arquitetura de três camadas e aderindo aos princípios **SOLID** para garantir manutenibilidade e escalabilidade.

## Testando a Aplicação Web

<details>
<summary>Image,</summary>

0. Abra o navegador e acesse o endereço onde o frontend está rodando (geralmente `http://localhost:63218/`).

1.  **Criar um usuário.**
2.  **Realizar login.** Para recueperar o token JWT necessário para autenticação nas operações de cliente.


3.  **Cadastrar um cliente.**
4.  **Listar clientes.**
5.  **Atualizar um cliente.**
6.  **Deletar um cliente.**
</details>




-----

### Pré-requisitos para Execução

Para rodar o projeto localmente, você precisará ter:

  * **.NET 8.0** instalado.
  * **Docker Desktop** instalado e em execução.
  * **node.js v22.19.0r** instalado (para o projeto web).

### Configuração do Banco de Dados MySQL no Docker

Siga estes passos para configurar seu banco de dados MySQL:

1.  **Puxar a imagem do MySQL:**
    ```bash
    docker pull mysql:8.0
    ```
2.  **Executar o contêiner do MySQL:**
    ```bash
    docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=Senha123! -e MYSQL_DATABASE=meuBanco -p 3306:3306 -d mysql:8.0
    ```
3.  **Configurar o usuário `hexagon`:**
      * Entre no contêiner MySQL:
        ```bash
        docker exec -it mysql-container mysql -u root -pSenha123!
        ```
      * Crie o usuário `hexagon`:
        ```sql
        CREATE USER 'hexagon'@'%' IDENTIFIED BY 'senhaHexagon';
        ```
      * Conceda todos os privilégios (similar ao root):
        ```sql
        GRANT ALL PRIVILEGES ON *.* TO 'hexagon'@'%' WITH GRANT OPTION;
        ```
      * Aplique as mudanças:
        ```sql
        FLUSH PRIVILEGES;
        ```

### Executando as Migrações Iniciais

Para aplicar as migrações do banco de dados:

```powershell
# Navegue até a pasta do projeto API (ajuste o caminho se necessário)
cd ../Hexagon.Api

# Execute os comandos de migração (no Package Manager Console do Visual Studio ou similar)
dotnet ef migrations add InicialMigration --project ../Hexagon.Api --startup-project ../Hexagon.Api --output-dir ../Hexagon.Api/Infrastruture/Data/Migrations
dotnet ef database update --project ../Hexagon.Api --startup-project ../Hexagon.Api
```

### Rodando a Aplicação Web

Para iniciar o projeto web:

1.  Abra o **Prompt de Comando como Administrador**.
2.  Navegue até a pasta do projeto web:
    ```bash
    cd Hexagon.Web
    ```
3.  Limpe o cache do npm, se necessário:
    ```bash
    npm cache clean --force
    ```
4.  Instale as dependências:
    ```bash
    npm install
    ```
5.  Execute o projeto:
    ```bash
    npm run dev
    ```
    O servidor será iniciado e exibirá o endereço para acesso ao site (geralmente `http://localhost:63218/`).


### Rodando a API

Execute o comando 
 ```bash
dotnet run --launch-profile "https"
   ```
no caminho: **..\Hexagon.Api**

Ou pelo Visual Studio:
No menu dropdown de inicialização (geralmente perto do botão "Start")

Clique em "Start"

-----


## Arquitetura e Princípios de Projeto

O projeto adota uma **arquitetura de três camadas**: Domínio, Apresentação e Infraestrutura.

  * A **camada de Domínio** contém as entidades e as regras de negócio centrais do sistema. O serviço de domínio reside aqui, definindo a lógica de negócio e orquestrando as interações com a camada de infraestrutura.
  * A **camada de Infraestrutura** é responsável pela persistência de dados (interação com o banco de dados) e integração com serviços externos.
  * A **camada de Apresentação** é responsável pela comunicação HTTP com a API e validaçõa dos modelos de entrada e montagem dos modelos de saída.

### Aplicação dos Princípios SOLID

A aderência aos princípios **SOLID** garante que o código seja organizado, fácil de manter e escalar:

  * **S (Single Responsibility Principle - Princípio da Responsabilidade Única):** Cada classe possui uma única responsabilidade bem definida.
      * **Controllers:** Lidam com requisições HTTP, validação de entrada e formatação de respostas, chamando os serviços apropriados.
      * **Serviços:** Implementam a lógica de negócio e orquestram operações entre controllers e repositórios.
      * **Repositórios:** Gerenciam a interação direta com o banco de dados para operações CRUD.
  * **O (Open/Closed Principle - Princípio Aberto/Fechado):** As classes são abertas para extensão, mas fechadas para modificação. O uso de **interfaces** para serviços e repositórios permite a adição de novas implementações (como a troca de banco de dados de MySQL para SQL Server) com modificações mínimas no código existente.
  * **L (Liskov Substitution Principle - Princípio de Substituição de Liskov):** Implementações de interfaces são substituíveis por outras sem afetar o funcionamento do sistema, garantindo flexibilidade e facilidade de manutenção.
  * **I (Interface Segregation Principle - Princípio da Segregação de Interfaces):** Interfaces são criadas de forma específica para cada cliente (serviço ou repositório), evitando interfaces genéricas e grandes, o que facilita o entendimento e a manutenção do código.
  * **D (Dependency Inversion Principle - Princípio da Inversão de Dependência):** Dependências são gerenciadas através de **injeção de dependência**, onde classes dependem de abstrações (interfaces) em vez de implementações concretas. Isso facilita a substituição de dependências e o teste do código.

A configuração de dependências utiliza o escopo **`scoped`**, garantindo que uma nova instância de serviço ou repositório seja criada para cada requisição HTTP, o que é crucial para evitar problemas de concorrência e manter a consistência dos dados.


### Teste unitário
O projeto inclui testes unitários para validar a lógica de negócio, utilizando frameworks como **xUnit** e **Moq**. Os testes cobrem os principais cenários, garantindo que os serviços funcionem conforme o esperado e que as regras de negócio sejam corretamente aplicadas impedindo alterações indevidas no código.

### Seguindo o básico de API Restful
A API foi projetada seguindo os princípios RESTful, utilizando os verbos HTTP adequados para cada operação:
  * **POST:** Para criar novos recursos (usuários e clientes).
  * **GET:** Para recuperar recursos (listar clientes).
  * **PUT:** Para atualizar recursos existentes (atualizar clientes).
  * **DELETE:** Para remover recursos (deletar clientes).

### Docker
Utilizei o docker para extrair a complexidade de instalação do banco de dados, e também para facilitar a portabilidade do projeto.

### Frontend com Vite e React
O frontend foi desenvolvido utilizando **Vite** como ferramenta de build e **React** para a construção da interface do usuário. O Vite proporciona um ambiente de desenvolvimento rápido (hot reload) e eficiente, enquanto o React permite a criação de componentes reutilizáveis e uma experiência de usuário dinâmica.

**Obs.:** Tenho conhecimento básico de HTML, CSS e JavaScript e um pouco de React, por isso, toda a estrutura do frontend foi criada a partir de comandos no LLM (ChatGPT), e posteriormente ajustada conforme a necessidade do projeto. Entendo um pouco de componentização, mas não utilizei todo o poder dos componentes por falta de prática e tempo de desenvolvimento.

### Outras Tecnologias Utilizadas

  * **Entity Framework Core:** Utilizado como ORM para facilitar a interação com o banco de dados MySQL e gerenciar migrações.
  * **FluentValidation:** Empregado para a validação robusta dos dados de entrada nos controllers, assegurando a integridade dos dados antes do processamento.

-----

## Testando a API

Um arquivo Postman com as requisições necessárias para testar o CRUD de usuários e clientes está disponível. As operações incluem:

1.  **Criar um usuário.**
2.  **Realizar login.** Para recueperar o token JWT necessário para autenticação nas operações de cliente.


3.  **Cadastrar um cliente.**
4.  **Listar clientes.**
5.  **Atualizar um cliente.**
6.  **Deletar um cliente.**


-----

## Considerações Finais

Este projeto demonstra a implementação de um sistema CRUD de usuário utilizando uma arquitetura de três camadas e princípios SOLID, garantindo um código limpo, organizado e fácil de manter. A utilização de Docker para o banco de dados e a construção do frontend com Vite e React proporcionam uma experiência de desenvolvimento moderna e eficiente. Sinta-se à vontade para explorar, modificar e expandir o projeto conforme suas necessidades!

Meu maior conhecimento é no backend, por isso o frontend está simples, mas funcional. Caso tenha dúvidas ou sugestões, estou aberto a feedbacks!