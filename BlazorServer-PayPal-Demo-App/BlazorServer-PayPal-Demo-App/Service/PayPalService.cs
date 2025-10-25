using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BlazorServer_PayPal_Demo_App.Service
{
    public class PayPalService
    {
        private readonly ILogger<PayPalService> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly bool _useSandbox;

        public PayPalService(ILogger<PayPalService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _clientId = configuration["PayPal:ClientId"] ?? throw new ArgumentNullException("PayPal:ClientId");
            _clientSecret = configuration["PayPal:ClientSecret"] ?? throw new ArgumentNullException("PayPal:ClientSecret");
            _useSandbox = bool.Parse(configuration["PayPal:UseSandbox"] ?? "true");
        }

        /// <summary>
        /// Creates a PayPal order
        /// </summary>
        /// <returns>Order creation response with order ID</returns>
        public async Task<OrderResponse> CreateOrderAsync(PaymentOrderRequest request)
        {
            try
            {
                // Create the order request
                var paypalOrderRequest = new OrdersCreateRequest();
                paypalOrderRequest.Prefer("return=representation");

                // Set request body
                paypalOrderRequest.RequestBody(
                    new OrderRequest()
                    {
                        CheckoutPaymentIntent = "CAPTURE",
                        PurchaseUnits = new List<PurchaseUnitRequest>
                        {
                            new()
                            {
                                AmountWithBreakdown = new AmountWithBreakdown
                                {
                                    CurrencyCode = request?.Currency,
                                    Value = request?.Amount.ToString()
                                },
                                Description = request?.Description
                            }
                        },
                        ApplicationContext = new ApplicationContext
                        {
                            ReturnUrl = request?.ReturnUrl,
                            CancelUrl = request?.CancelUrl
                        }
                    });

                // Get PayPal client
                PayPalHttpClient client = GetPayPalClient();

                // Execute the request
                var response = await client.Execute(paypalOrderRequest);
                var paypalOrder = response.Result<Order>();

                return new OrderResponse
                {
                    Id = paypalOrder.Id,
                    Status = paypalOrder.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PayPal order");
                throw;
            }
        }

        /// <summary>
        /// Captures a PayPal order after approval
        /// </summary>
        /// <param name="orderId">The PayPal order ID to capture</param>
        /// <returns>Order capture response</returns>
        public async Task<CaptureResponse> CaptureOrderAsync(string orderId)
        {
            try
            {
                // Create capture request
                var request = new OrdersCaptureRequest(orderId);
                request.Prefer("return=representation");
                request.RequestBody(new OrderActionRequest());

                // Get PayPal client
                PayPalHttpClient client = GetPayPalClient();

                // Execute the request
                var response = await client.Execute(request);
                var capturedOrder = response.Result<Order>();

                return new CaptureResponse
                {
                    Id = capturedOrder.Id,
                    Status = capturedOrder.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error capturing PayPal order");
                throw;
            }
        }

        private PayPalHttpClient GetPayPalClient()
        {
            PayPalEnvironment environment = _useSandbox 
                ? new SandboxEnvironment(_clientId, _clientSecret) 
                : new LiveEnvironment(_clientId, _clientSecret);
            
            var payPalHttpClient = new PayPalHttpClient(environment);
            return payPalHttpClient;
        }
    }

    // Response models
    public class OrderResponse
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
    }

    public class CaptureResponse
    {
        public string? Id { get; set; }
        public string? Status { get; set; }
    }

    // Request models
    public class PaymentOrderRequest
    {
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? ReturnUrl { get; set; }
        public string? CancelUrl { get; set; }
    }

    public class CaptureRequest
    {
        public string? OrderId { get; set; }
    }
}
