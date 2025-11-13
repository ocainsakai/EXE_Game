using System;
using System.Collections.Generic;

[System.Serializable]
public class PayOSCreateOrderRequest
{
    public string productId;
    public int quantity;
    public int amount;
    public string userId;
}

[System.Serializable]
public class PayOSCreateOrderResponse
{
    public string orderId;
    public string paymentUrl;
}

[System.Serializable]
public class PayOSOrderStatusResponse
{
    public string orderId;
    public string status;
    public string productId;
}