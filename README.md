# teste-de-habilidade-hexagon
Será criado um CRUD para uma tabela de usuário.

Link do desafio: [Link PRIVADO](https://drive.google.com/file/d/1ebhn52BzlPTXM2EndGbx0PhWrwvhj3dp/view?usp=drive_link)

#### Comandos

**Para criar e rodar localmente o SQL Server:**

docker pull mysql:8.0
docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=Senha123! -e MYSQL_DATABASE=meuBanco -p 3306:3306 -d mysql:8.0

**Para configurar a senha e o usuário hexagon:**
**Entre no container MySQL:**

docker exec -it mysql-container mysql -u root -pSenha123!

**Criar o usuário hexagon**
CREATE USER 'hexagon'@'%' IDENTIFIED BY 'senhaHexagon';

**Conceder todos os privilégios em todos os bancos de dados (igual ao root)**
GRANT ALL PRIVILEGES ON *.* TO 'hexagon'@'%' WITH GRANT OPTION;

**Aplicar as mudanças**
FLUSH PRIVILEGES;

**Para rodar a migração inicial:**

dotnet ef migrations add InicialMigration --project ../Hexagon.Api --startup-project ../Hexagon.Api --output-dir ../Hexagon.Api/Data/Migrations

dotnet ef database update --project ../Hexagon.Api --startup-project ../Hexagon.Api


**Para Rodar o web**

# Abra o Command Prompt como Administrador

# Navegue até a pasta
cd Hexagon.Web

# Limpe cache se necessário
npm cache clean --force

# Instale dependências
npm install

# Execute o projeto
npm run dev

# O servidor vai iniciar e mostrar:
# ➜  Local:   http://localhost:63218/
# ➜  Network: use --host to expose


**Rascunho de explicação do projeto*

Seguindo um padrão de projeto em 3 camadas, temos a camada de dominio, aplicação e infraestrutura. [link](https://macoratti.net/21/08/net_arq3layer1.htm)

1 - A camada de domínio é responsável por conter as entidades e regras de negócio. 
2 - O Serviço fica na camada de domínio pois é um serviço de domínio e ele é responsável por definir as regras de negócio para cada ação do sistema e orquestrar a ligação com a camada de infraestrutura. 
3 - A camada de infraestrutura é responsável por interagir com o banco de dados e outros serviços externos.

Utilizei os princípios SOLID para garantir que o código seja fácil de manter e escalar.

S - Principio de responsabilidade única: Cada classe tem uma única responsabilidade.
 O Controller, é responsável por lidar com as requisições HTTP e chamar os serviços apropriados. Além disso no controller, é feita a validação dos dados de entrada e a formatação da resposta.
 O Serviço, é responsável por implementar a lógica de negócio e orquestrar as operações entre o repositório e o controller.
 O Repositório, é responsável por interagir com o banco de dados e realizar as operações CRUD.

 Conseguindo assim manter o código organizado e fácil de entender utilizando o principio de responsabilidade única.

 O - Princípio aberto/fechado: As classes devem ser abertas para extensão, mas fechadas para modificação.
  Utilizei interfaces para definir contratos para os serviços e repositórios, permitindo que novas implementações possam ser adicionadas sem modificar o código existente. Ou seja eu não tenho que modificar o código existente para adicionar novas funcionalidades, apenas criar novas classes que implementem as interfaces existentes se eu precisar trocar o banco de dados por exemplo de MySQl Para SQL Server, a modificação seria mínima.


  L - Princípio de substituição de Liskov: As subclasses devem ser substituíveis por suas classes base.
  As classes que implementam as interfaces podem ser substituídas por outras implementações sem afetar o funcionamento do sistema. Isso garante que o código seja flexível e fácil de manter ou seja eu posso trocar a implementação do repositório sem afetar o serviço ou o controller.
  
  I - Princípio da segregação de interfaces: As interfaces devem ser específicas para o cliente.
	Estou criando interfaces que são específicas para cada serviço ou repositório, evitando interfaces grandes e genéricas. Isso torna o código mais fácil de entender e manter.

  D - Princípio da inversão de dependência: As classes devem depender de abstrações, não de implementações concretas.
  Utilizei injeção de dependência para fornecer as implementações dos serviços e repositórios para os controllers e services/repository também utilizaram a injeção de depêndencia. Isso permite que as dependências sejam facilmente substituídas, facilitando testes e manutenção do código.

  Utilizei o tipo scoped para os serviços e repositórios, garantindo que uma nova instância seja criada para cada requisição HTTP. Isso é importante para evitar problemas de concorrência e garantir que os dados sejam consistentes durante a execução de uma requisição.

  Utilizei o Entity Framework Core como ORM para interagir com o banco de dados MySQL. O EF Core facilita a manipulação dos dados e a criação das migrações para manter o banco de dados atualizado com as mudanças no modelo de dados.

  Utilizei o FluentValidation para validar os dados de entrada nos controllers. Isso garante que os dados sejam válidos antes de serem processados pelos serviços, evitando erros e inconsistências no sistema.

  #### Como utilizar via API e WEB

  Segue anexo um arquivo Postman com as requisições para testar.

  1 - Criar um usuário
  2 - Fazer login
  3 - Cadastrar um cliente
  4 - Listar clientes
  5 - Atualizar um cliente
  6 - Deletar um cliente



