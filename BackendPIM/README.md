# ConectaLar - Banco de Dados (PIM)

Este repositório contém a camada de persistência de dados e a estrutura de banco de dados do projeto **ConectaLar** — uma plataforma integrada para centralização e gerenciamento de serviços de manutenção residencial, desenvolvida como Projeto Integrado Multidisciplinar (PIM).

---

## Tecnologias Utilizadas

* **ASP.NET Core:** Estrutura principal do backend para construção de APIs REST.
* **Entity Framework Core (EF Core):** ORM utilizado para mapeamento objeto-relacional através da abordagem *Code First*.
* **SQLite:** Banco de dados relacional leve, baseado em arquivo local (`ConectaLar.db`), ideal para portabilidade acadêmica e manutenibilidade.

---

## Estrutura das Tabelas (Mapeamento do Banco)

O banco de dados foi estruturado e blindado utilizando *Data Annotations* (tamanhos máximos de campos), restrições de integridade e regras de consistência de negócio diretamente no SQLite:

* **Módulo de Autenticação e Perfis (Segurança Avançada):** 
  * `Usuarios` (Central de logins protegida com criptografia/Hash real e controle estrito por nível de acesso via Enums: *Cliente, Profissional ou Administrador*).
  * **Restrição 1:1:** Configurada via Fluent API para garantir que uma conta de acesso (`Usuario`) pertença a apenas um único Cliente ou Profissional, impedindo duplicidade de perfis.
  * **E-mail Único:** Índice exclusivo configurado no banco para bloquear cadastros com o mesmo e-mail.

* **Módulo Principal (Core de Negócio):**
  * `Clientes` (Dados cadastrais vinculados à sua respectiva conta de acesso).
  * `Profissionais` (Dados cadastrais, especialidades de manutenção e status de disponibilidade em tempo real).
  * `Servicos` (Catálogo de serviços e manutenções disponíveis com valores base, protegido contra estouro de caracteres).
  * `Agendamentos` (Tabela central que correlaciona Cliente, Profissional e Serviço. Conta com campo de controle `AceitoPeloProfissional` e proteção via `DeleteBehavior.Restrict` contra exclusões em cascata, preservando o histórico financeiro da empresa).
  * `Avaliacoes` (Sistema de feedback pós-serviço, limitando notas rigidamente de 1 a 5 estrelas através de validação nativa de intervalo).

* **Módulo de Responsabilidade Social (Leis 10.639 e 11.645):**
  * `ConteudosDiversidade` (Artigos e treinamentos corporativos de inclusão com categorização estrita por Enum).
  * `ParticipacoesTreinamentos` (Histórico e auditoria de conclusão de cursos por parte da equipe).
  * `RelatosDiscriminacao` (Ouvidoria interna e canal de denúncias contra preconceito com suporte a relatos anônimos. Blindado na camada de dados com a trava `[Required]`, impossibilitando o envio de relatos sem conteúdo textual).


---

## Como Executar o Banco na sua Máquina

1. Clone o repositório para o seu computador.
2. Certifique-se de ter o **SDK do .NET 8 ou superior** instalado.
3. Abra o projeto no **Visual Studio**.
4. Abra o **Console do Gerenciador de Pacotes** (*Package Manager Console*):
   * `Ferramentas` > `Gerenciador de Pacotes NuGet` > `Console do Gerenciador de Pacotes`
5. Execute o seguinte comando para criar o arquivo físico do banco de dados contendo todas as tabelas:
   ```powershell
   Update-Database
   ```
6. O arquivo `ConectaLar.db` nascerá na raiz do seu projeto automaticamente, já populado com dados de teste.

---

## Credenciais Padrão de Teste (Data Seeding)

O banco de dados já possui registros automáticos injetados para facilitar os testes iniciais das APIs e do Painel Web:

* **Administrador Padrão:**
  * **E-mail:** `admin@conectalar.com`
  * **Senha:** `admin123`
