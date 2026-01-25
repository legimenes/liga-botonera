namespace LigaBotonera.Pages.Shared.ModalDialog;

public record ModalDialogViewModel(
    ModalDialogType Type,
    string Title,
    IEnumerable<string> Messages
);
