namespace LigaBotonera.ViewComponents.MessageModal;
public record MessageModalViewModel(
    MessageModalType Type,
    string Title,
    IEnumerable<string> Messages
);