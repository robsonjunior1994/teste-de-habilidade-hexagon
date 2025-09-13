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