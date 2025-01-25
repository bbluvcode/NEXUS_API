using NEXUS_API.Models;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Microsoft.Extensions.Options;

namespace NEXUS_API.Service
{
    public class PayPalService
    {
        // Biến để lưu cấu hình PayPal từ cấu hình ứng dụng
        private readonly PayPalConfig _payPalConfig;
        // Biến để lưu đối tượng PayPalHttpClient, dùng để thực hiện các yêu cầu API
        private readonly PayPalHttpClient _client;
        // Constructor để khởi tạo đối tượng PayPalService
        public PayPalService(IOptions<PayPalConfig> payPalConfig)
        {
            // Gán giá trị cấu hình PayPal từ IOptions (có thể lấy từ appsettings.json)
            _payPalConfig = payPalConfig.Value;
            // Cấu hình môi trường PayPal (sandbox hoặc live)
            PayPalEnvironment environment;
            if (_payPalConfig.Mode == "sandbox")
            {
                // Nếu chế độ là sandbox, tạo môi trường sandbox với ClientId và ClientSecret từ cấu hình
                environment = new SandboxEnvironment(_payPalConfig.ClientId, _payPalConfig.ClientSecret);
            }
            else
            {
                // Nếu chế độ là live, tạo môi trường live với ClientId và ClientSecret từ cấu hình
                environment = new LiveEnvironment(_payPalConfig.ClientId, _payPalConfig.ClientSecret);
            }

            // Tạo đối tượng PayPalHttpClient để thực hiện các API request
            _client = new PayPalHttpClient(environment);
        }

        // Hàm tạo thanh toán PayPal
        public async Task<Order> CreatePayment(decimal totalAmount)
        {
            // Tạo yêu cầu tạo thanh toán với các tham số cơ bản
            // (checkout payment intent và đơn hàng)
            var request = new OrdersCreateRequest()
                .RequestBody(new OrderRequest
                {
                    // Đặt phương thức thanh toán
                    // (CAPTURE có nghĩa là thanh toán sẽ được xử lý ngay lập tức)
                    CheckoutPaymentIntent = "CAPTURE",

                    // Đặt các thông tin về đơn hàng, bao gồm số tiền và đơn vị tiền tệ
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown  = new AmountWithBreakdown
                        {
                            CurrencyCode  = "USD",  // Mã tiền tệ (USD)
                            Value  = totalAmount.ToString()  // Giá trị của đơn hàng
                        }
                    }
                    },

                    // Đặt URL trả về nếu thanh toán thành công
                    // và URL hủy nếu thanh toán bị hủy
                    ApplicationContext = new ApplicationContext
                    {
                        // URL trả về nếu thanh toán thành công
                        ReturnUrl = "http://localhost:3000/success-deposit",
                        // URL hủy nếu người dùng hủy thanh toán
                        CancelUrl = "http://localhost:3000/500"
                    }
                });

            try
            {
                // Gửi yêu cầu tạo thanh toán đến PayPal
                var response = await _client.Execute(request);

                // Lấy kết quả trả về từ yêu cầu (thông tin đơn hàng)
                var order = response.Result<Order>();

                // Trả về thông tin đơn hàng nếu thanh toán được tạo thành công
                return order;
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra trong quá trình tạo thanh toán, ném lỗi ra ngoài
                throw new Exception($"Error creating PayPal payment {ex.Message}");
            }
        }

        // Hàm xử lý thanh toán khi người dùng đồng ý
        public async Task<Order> CapturePayment(string orderId)
        {
            // Tạo yêu cầu capture thanh toán dựa trên orderId
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());

            try
            {
                // Gửi yêu cầu capture thanh toán đến PayPal
                var response = await _client.Execute(request);

                // Lấy kết quả trả về từ yêu cầu
                // (thông tin đơn hàng sau khi thanh toán được capture)
                var order = response.Result<Order>();

                // Kiểm tra trạng thái của đơn hàng sau khi capture
                if (order.Status == "COMPLETED")
                {
                    // Nếu trạng thái là "COMPLETED", trả về đơn hàng
                    return order;
                }
                else
                {
                    // Nếu trạng thái không phải "COMPLETED", ném lỗi ra ngoài
                    throw new Exception("Order capture failed, status: " + order.Status);
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra trong quá trình capture thanh toán,
                // ném lỗi ra ngoài
                throw new Exception("Error capturing PayPal payment", ex);
            }
        }
    }

}
