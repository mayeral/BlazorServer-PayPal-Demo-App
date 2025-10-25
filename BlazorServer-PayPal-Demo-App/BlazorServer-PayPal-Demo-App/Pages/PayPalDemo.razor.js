// Blazor Page Script for PayPal integration
// This is automatically scoped to the PayPalDemo.razor component

export function loadPayPalScript(dotNetCallback, clientId, amount, currency, description, baseUrl) {
    // Remove any existing PayPal script
    const existingScript = document.getElementById('paypal-script');
    if (existingScript) {
        existingScript.remove();
    }

    // Clear the container
    const container = document.getElementById('paypal-button-container');
    if (container) {
        container.innerHTML = '';
    }

    // Create and load the PayPal script
    const script = document.createElement('script');
    script.id = 'paypal-script';
    script.src = `https://www.paypal.com/sdk/js?client-id=${clientId}&currency=${currency}`;

    // Set up the PayPal button when script loads
    script.onload = function () {
        window.paypal.Buttons({
            // Styling options
            style: {
                shape: 'pill',
                color: 'blue',
                layout: 'vertical'
            },

            // Create the order
            createOrder: async function () {
                try {
                    // Call the .NET method to create the order
                    const orderData = await dotNetCallback.invokeMethodAsync('CreateOrderAsync', {
                        amount: amount,
                        currency: currency,
                        description: description,
                        returnUrl: baseUrl + 'paypal-demo?success=true',
                        cancelUrl: baseUrl + 'paypal-demo?cancel=true'
                    });

                    // Return JUST the ID string, not the object
                    if (orderData && orderData.id) {
                        return orderData.id;
                    } else {
                        throw new Error('No order ID received from server');
                    }
                }
                catch (error) {
                    console.error('Error creating order:', error);
                    dotNetCallback.invokeMethodAsync('OnPaymentError', error.toString());
                    throw error;
                }
            },

            // Callback for when the payment is approved
            onApprove: function (data) {
                return dotNetCallback.invokeMethodAsync('OnPaymentApproved', data.orderID);
            },

            // Callback for when the payment is cancelled
            onCancel: function () {
                return dotNetCallback.invokeMethodAsync('OnPaymentCancelled');
            },

            // Callback for when the payment fails
            onError: function (err) {
                console.error('PayPal error:', err);
                return dotNetCallback.invokeMethodAsync('OnPaymentError', err.toString());
            }
        }).render('#paypal-button-container');
    };

    script.onerror = function () {
        const errorMsg = 'Failed to load PayPal SDK script';
        console.error(errorMsg);
        dotNetCallback.invokeMethodAsync('OnPaymentError', errorMsg);
    };

    document.body.appendChild(script);
}
