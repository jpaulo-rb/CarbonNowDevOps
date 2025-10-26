Feature: Recebimento de dados de eletricidade
  Para manter um histórico confiável de consumo elétrico
  Como sistema ESG
  Eu quero receber e armazenar dados de eletricidade no banco de dados

  Scenario: Armazenar dado de eletricidade válido
    Given que recebi um dado de eletricidade com valor de eletricidade "120"
    When o sistema processar o registro de eletricidade
    Then o dado de eletricidade deve ser salvo no banco de dados e retornar status "201"

  Scenario: Rejeitar dado de eletricidade com valor de eletricidade inválido
    Given que recebi um dado de eletricidade com valor de eletricidade "-50"
    When o sistema processar o registro de eletricidade
    Then o sistema deve rejeitar o dado de eletricidade e retornar status "400" com mensagem "Valor de eletricidade inválido"

  Scenario: Rejeitar dado de eletricidade com valor de carbono inválido
    Given que recebi um dado de eletricidade com valor de carbono "-50" kg
    When o sistema processar o registro de eletricidade
    Then o sistema deve rejeitar o dado de eletricidade e retornar status "400" com mensagem "Valor de carbono inválido"
