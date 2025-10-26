Feature: Recebimento de dados de transporte
  Para manter um histórico confiável de transportes
  Como sistema ESG
  Eu quero receber e armazenar dados de transportes no banco de dados

  Scenario: Armazenar transporte válido
    Given que recebi um transporte com distância "120" km
    When o sistema processar o registro de transporte
    Then o dado de transporte deve ser salvo no banco de dados e retornar status "201"

  Scenario: Rejeitar transporte com distância inválida
    Given que recebi um transporte com distância "-50" km
    When o sistema processar o registro de transporte
    Then o sistema deve rejeitar o dado de transporte e retornar status "400" com mensagem "Distância inválida"

  Scenario: Rejeitar transporte com peso inválido
    Given que recebi um transporte com peso "-50" kg
    When o sistema processar o registro de transporte
    Then o sistema deve rejeitar o dado de transporte e retornar status "400" com mensagem "Peso inválido"