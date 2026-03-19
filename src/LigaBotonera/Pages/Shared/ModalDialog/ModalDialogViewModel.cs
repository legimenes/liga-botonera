namespace LigaBotonera.Pages.Shared.ModalDialog;

public record ModalDialogViewModel(
    ModalDialogType Type,
    IEnumerable<string> Messages,
    string? Title = null,
    string? PostBackUrl = null
);
