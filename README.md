# Serviços - envio de e-mail com worker e RabbitMQ
Projeto de serviços para receber contatos em uma aplicação MVC, salvar no SQL Server, publicar mensagem no RabbitMQ e um worker consumindo mensagens do RabbitMQ e enviando e-mail através do Sendgrid.

## Sobre o desenvolvimento
Durante o desenvolvimento desse projeto tive a oportunidade de criar um worker, aplicação que eu não conhecia. Entre os principais desafios destaco a integração dos services entre si, lidando com diferentes ciclos de vida de serviço. Embora seja uma aplicação simples, precisei utilizar a interface IServiceScopeFactory para criar novas instâncias da conexão com o banco de dados a cada iteração do worker.

Já no desenvolvimento do projeto FormContact consegui executar melhor a separação dos services, bem como entender a dinâmica do trabalho com aplicações MVC, com o retorno de views nos controllers.

## Worker
![image](https://github.com/carolrochafloro/microservices-email-rabbitmq/assets/127871333/6e89de4a-5b3d-47c4-bcea-e7118c62ab0d)

## Backlog
- Testes;
- Tela de cadastro;
- Tela de login;
- Tela de visualização de mensagens;
- Docker-compose: SQL Server, RabbitMQ, projeto Email e projeto FormContato;
- Paginação de resultados.
