public interface IUIElement
{
    void Init();
    void Show(object[] data);
    void Hide();
    void Update();
    void Dispose();
}