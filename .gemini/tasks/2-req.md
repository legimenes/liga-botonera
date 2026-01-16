# Display a window with errors inside ModalContainer component

A página **Pages/Clubs/Edit** quando está renderizada no componente **ModalContainer** e acontece algum erro de validação ao clicar no botão **Salvar**, deve exibir o componente **MessageModal** com os respectivos erros retornados na linha **ValidationResult validationResult = new Validator().Validate(Data);**.

