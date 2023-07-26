function fetchTransactions() {
    const transactionKind = $('#transaction-kind').val();
    const page = $('#page').val();
    const pageSize = $('#page-size').val();
    const sortOrder = $('#sort-order').val();
    const sortBy = $('#sort-by').val();
    const startDate = $('#start-date').val();
    const endDate = $('#end-date').val();

    $.ajax({
        url: `https://localhost:7087/transactions?transaction-kind=${transactionKind}&page=${page}&page-size=${pageSize}&sort-order=${sortOrder}&sort-by=${sortBy}&start-date=${startDate}&end-date=${endDate}`,
        method: 'GET',
        success: function(data) {
            $('#transactions-table tbody').empty();
			$('#transaction-id').empty();
            data.items.forEach(transaction => {
                let date = new Date(transaction.date);
                let formattedDate = date.toLocaleDateString();
                let splitMarker = transaction.splits ? transaction.splits.length > 0 : false;
                let row = `
                    <tr>
                        <td>${transaction.id}</td>
                        <td>${transaction.beneficiaryName}</td>
                        <td>${formattedDate}</td>
                        <td>${transaction.direction}</td>
                        <td>${transaction.amount}</td>
                        <td>${transaction.currency}</td>
                        <td>${transaction.kind}</td>
                        <td>${splitMarker}</td>
                        <td>${transaction.description}</td>
						<td>${transaction.catcode}</td>
                    </tr>
                `;
				console.log(transaction)
                $('#transactions-table tbody').append(row);
				$('#transaction-id').append(`<option value="${transaction.id}">${transaction.id}</option>`);  // Populate the transaction ID dropdown
            });
        },
        error: function(error) {
            console.error(error);
        }
    });
}

$(document).ready(function() {
    fetchTransactions();
    $('input').on('change', fetchTransactions);
});
