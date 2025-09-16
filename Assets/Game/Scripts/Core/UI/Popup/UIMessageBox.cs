using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIMessageBox : UIPopup
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI MessageText;
    public Button OkButton;
    public Button CancelButton;

    // Parameter cho MessageBox
    public class MessageBoxParam
    {
        public string Title;
        public string Message;
        public Action OnOk;
        public Action OnCancel;
    }

    private MessageBoxParam Param => Parameter as MessageBoxParam;

    protected override void OnInit()
    {
        base.OnInit();

        // Nếu được tạo bằng DefaultPopupFactory, có thể đã có sẵn Text & Button
        if (OkButton != null)
            OkButton.onClick.AddListener(OnOkClick);
        if (CancelButton != null)
            CancelButton.onClick.AddListener(OnCancelClick);
    }

    protected override void OnShowing()
    {
        base.OnShowing();
        if (Param != null)
        {
            if (TitleText != null) TitleText.text = Param.Title ?? "Message";
            if (MessageText != null) MessageText.text = Param.Message ?? "";
        }
    }

    private void OnOkClick()
    {
        Param?.OnOk?.Invoke();
        Hide();
    }

    private void OnCancelClick()
    {
        Param?.OnCancel?.Invoke();
        Hide();
    }
}