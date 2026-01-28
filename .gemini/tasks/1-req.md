# Fix overlap behavior between _ModalContainer and _ModalDialog
 
Quando a partial view _ModalDialog é renderizada e a partial view _ModalContainer já está renderizada na tela, a _ModalContainer está sendo fechada. O comportamento que espero é que _ModalContainer continue renderizada por baixo da _ModalDialog.

Analise a implementação dos métodos OnGetForm e OnPostSave de Clubs/Index para entender como está a implementação hoje para entender onde deve ser aplicada a correção.