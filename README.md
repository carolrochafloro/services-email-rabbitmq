# Serviços - envio de e-mail com worker e RabbitMQ
Projeto de serviços para receber contatos em uma aplicação MVC, salvar no SQL Server, publicar mensagem no RabbitMQ e um worker consumindo mensagens do RabbitMQ e enviando e-mail através do Sendgrid.

## Funcionalidades
A proposta do projeto é ser um gerenciador de contados enviados através de um formulário. O usuário logado informa o e-mail para onde quer enviar as mensagens e recebe uma URL personalizada, que direciona para um formulário de contato com campos de nome, e-mail e mensagem.

Após o envio do formulário, os dados do recebedor e da mensagem são publicados no RabbitMQ e consumidos pelo worker, que utiliza o Sendgrid para enviar a mensagem para o destinatário correto e atualiza o banco de dados em caso de sucesso.

No dashboard o usuário pode visualizar as mensagens enviadas através do formulário e o status de envio do e-mail.

## Sobre o desenvolvimento
Durante o desenvolvimento desse projeto tive a oportunidade de criar um worker, aplicação que eu não conhecia. Entre os principais desafios destaco a integração dos services entre si, lidando com diferentes ciclos de vida de serviço. Embora seja uma aplicação simples, precisei utilizar a interface IServiceScopeFactory para criar novas instâncias da conexão com o banco de dados a cada iteração do worker.

Já no desenvolvimento do projeto FormContact consegui executar melhor a separação dos services, bem como entender a dinâmica do trabalho com aplicações MVC, com o retorno de views nos controllers.

## Worker
![image](https://github.com/carolrochafloro/microservices-email-rabbitmq/assets/127871333/6e89de4a-5b3d-47c4-bcea-e7118c62ab0d)

## Backlog
- Testes;
- Docker-compose: SQL Server, RabbitMQ, projeto Email e projeto FormContato;
- Paginação de resultados;
- Melhorar tratamento de erros;
- Implementar logging no DB.

