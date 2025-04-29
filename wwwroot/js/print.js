window.openPrintWindow = function (data) {
    // Force a new window with specific settings to avoid browser blocking
    const printWindow = window.open('about:blank', '_blank', 'width=800,height=600,scrollbars=yes');
    console.log(data);
    
    if (!printWindow) {
        alert('Please allow popups for this website to print the bill');
        return false;
    }
    
    const today = new Date();
    const formattedDate = today.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
    const invoiceNumber = `INV-${today.getFullYear()}${(today.getMonth()+1).toString().padStart(2, '0')}${today.getDate().toString().padStart(2, '0')}-${Math.floor(Math.random() * 1000).toString().padStart(3, '0')}`;
    
    const content = `
        <!DOCTYPE html>
        <html>
        <head>
            <title>Invoice - Table ${data.table}</title>
            <style>
                body { 
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
                    padding: 20px; 
                    color: #333; 
                    max-width: 800px; 
                    margin: 0 auto; 
                }
                .invoice-container {
                    border: 1px solid #ddd;
                    padding: 20px;
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                }
                .logo-container {
                    text-align: center;
                    margin-bottom: 10px;
                }
                .logo {
                    font-size: 40px;
                    color: #4a6741;
                    margin: 0;
                }
                .restaurant-name {
                    font-size: 24px;
                    font-weight: bold;
                    margin: 5px 0;
                    color: #4a6741;
                }
                .header { 
                    display: flex;
                    justify-content: space-between;
                    border-bottom: 2px solid #4a6741;
                    padding-bottom: 10px;
                    margin-bottom: 20px;
                }
                .header-left, .header-right {
                    flex: 1;
                }
                .header-right {
                    text-align: right;
                }
                .invoice-details {
                    margin-bottom: 20px;
                    padding: 10px;
                    background-color: #f9f9f9;
                    border-radius: 5px;
                }
                .invoice-details p {
                    margin: 5px 0;
                }
                .table { 
                    width: 100%; 
                    border-collapse: collapse; 
                    margin-bottom: 20px; 
                }
                .table th, .table td { 
                    padding: 12px 8px; 
                    text-align: left; 
                }
                .table th { 
                    background-color: #4a6741; 
                    color: white;
                    font-weight: normal;
                }
                .table tbody tr:nth-child(odd) {
                    background-color: #f9f9f9;
                }
                .table tbody tr:hover {
                    background-color: #f1f1f1;
                }
                .table tbody td {
                    border-bottom: 1px solid #ddd;
                }
                .text-right { 
                    text-align: right; 
                }
                .total-section {
                    margin-top: 20px;
                    border-top: 1px solid #ddd;
                    padding-top: 10px;
                }
                .total-row {
                    display: flex;
                    justify-content: flex-end;
                    margin: 5px 0;
                }
                .total-label {
                    width: 150px;
                    text-align: right;
                    padding-right: 20px;
                }
                .total-value {
                    width: 100px;
                    text-align: right;
                    font-weight: bold;
                }
                .grand-total {
                    font-size: 18px;
                    color: #4a6741;
                    border-top: 2px solid #4a6741;
                    padding-top: 5px;
                    margin-top: 5px;
                }
                .special-instructions { 
                    margin-top: 20px; 
                    padding: 15px; 
                    background-color: #fff3cd; 
                    border-left: 4px solid #ffc107;
                    border-radius: 4px;
                }
                .footer {
                    margin-top: 30px;
                    text-align: center;
                    font-size: 14px;
                    color: #777;
                    border-top: 1px solid #ddd;
                    padding-top: 15px;
                }
                .footer p {
                    margin: 5px 0;
                }
                .thank-you {
                    font-size: 18px;
                    color: #4a6741;
                    margin-bottom: 10px;
                }
                .divider {
                    height: 1px;
                    background-color: #ddd;
                    margin: 15px 0;
                }
                @media print {
                    body { 
                        padding: 0; 
                        font-size: 12px;
                    }
                    .no-print { 
                        display: none; 
                    }
                    .invoice-container {
                        border: none;
                        box-shadow: none;
                    }
                }
            </style>
        </head>
        <body>
            <div class="invoice-container">
                <div class="logo-container">
                    <div class="logo">🍽️</div>
                    <div class="restaurant-name">GOURMET DELIGHT</div>
                    <div>Fine Dining Experience</div>
                </div>
                
                <div class="header">
                    <div class="header-left">
                        <p><strong>Gourmet Delight Restaurant</strong></p>
                        <p>123 Culinary Avenue</p>
                        <p>Foodie City, FC 12345</p>
                        <p>Tel: (555) 123-4567</p>
                    </div>
                    <div class="header-right">
                        <h2>INVOICE</h2>
                        <p><strong>Invoice #:</strong> ${invoiceNumber}</p>
                        <p><strong>Date:</strong> ${formattedDate}</p>
                        <p><strong>Time:</strong> ${data.time}</p>
                    </div>
                </div>
                
                <div class="invoice-details">
                    <p><strong>Table:</strong> ${data.table}</p>
                    <p><strong>Server:</strong> Your Server</p>
                    <p><strong>Payment Method:</strong> Cash/Card</p>
                </div>
                
                <table class="table">
                    <thead>
                        <tr>
                            <th>Item Description</th>
                            <th class="text-right">Qty</th>
                            <th class="text-right">Unit Price</th>
                            <th class="text-right">Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${data.items.map(item => `
                            <tr>
                                <td>${item.name}</td>
                                <td class="text-right">${item.quantity}</td>
                                <td class="text-right">$${item.price.toFixed(2)}</td>
                                <td class="text-right">$${item.subtotal.toFixed(2)}</td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
                
                <div class="total-section">
                    <div class="total-row">
                        <div class="total-label">Subtotal:</div>
                        <div class="total-value">$${data.total.toFixed(2)}</div>
                    </div>
                    <div class="total-row">
                        <div class="total-label">Tax (0%):</div>
                        <div class="total-value">$0.00</div>
                    </div>
                    <div class="total-row grand-total">
                        <div class="total-label">TOTAL:</div>
                        <div class="total-value">$${data.total.toFixed(2)}</div>
                    </div>
                </div>

                ${data.specialInstructions ? `
                    <div class="special-instructions">
                        <strong>Special Instructions:</strong>
                        <p>${data.specialInstructions}</p>
                    </div>
                ` : ''}
                
                <div class="divider"></div>
                
                <div class="footer">
                    <p class="thank-you">Thank You for Dining with Us!</p>
                    <p>We appreciate your business and hope to see you again soon.</p>
                    <p>www.gourmetdelight.example.com</p>
                </div>
            </div>

            <div class="no-print" style="text-align: center; margin-top: 20px;">
                <button onclick="window.print()" style="padding: 10px 20px; background-color: #4a6741; color: white; border: none; border-radius: 4px; cursor: pointer;">Print Invoice</button>
            </div>
        </body>
        </html>
    `;
    console.log(content);
    
    printWindow.document.write(content);
    printWindow.document.close();
    
    // Give the browser a moment to render the content before printing
    setTimeout(function() {
        printWindow.focus(); // Focus on the print window
        printWindow.print(); // Automatically trigger the print dialog
    }, 500);
    
    return true;
};