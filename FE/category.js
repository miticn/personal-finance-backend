$(document).ready(function() {
    // Fetch all categories on page load
    fetchCategories();

    // When a category is selected, fetch its subcategories
    $('#category').change(function() {
        const categoryCode = $(this).val();
        fetchSubcategories(categoryCode);
    });

    // Apply category to transaction when the Apply button is clicked
    $('#apply').click(function() {
		const transactionId = $('#transaction-id').val();  // Get the selected transaction ID
		const subcategoryCode = $('#subcategory').val();
		const categoryCode = $('#category').val();
		categorizeTransaction(transactionId, categoryCode, subcategoryCode);
	});
});

function fetchCategories() {
    $.ajax({
        url: 'https://localhost:7087/categories',
        method: 'GET',
        success: function(data) {
            $('#category').empty();
            data.forEach(function(category) {
                // Only add the top-level categories to the dropdown
                
                const option = `<option value="${category.code}">${category.name}</option>`;
                $('#category').append(option);
            });
        }
    });
}

function fetchSubcategories(categoryCode) {
    $.ajax({
        url: `https://localhost:7087/categories?parent-id=${categoryCode}`,
        method: 'GET',
        success: function(data) {
			console.log(data)
            $('#subcategory').empty();
			const option = `<option value="none">No subcategory</option>`;
			$('#subcategory').append(option);
            data.forEach(function(category) {
                // Only add the subcategories of the selected category to the dropdown
                const option = `<option value="${category.code}">${category.name}</option>`;
                $('#subcategory').append(option);
                
            });
        }
    });
}

function categorizeTransaction(transactionId, categoryCode, subCategoryCode) {
    $.ajax({
        url: `https://localhost:7087/transactions/${transactionId}/categorize`,
        method: 'POST',
        data: JSON.stringify({
            catcode: subCategoryCode=="none"?categoryCode:subCategoryCode
        }),
        contentType: 'application/json',
        success: function() {
            alert('Category applied successfully!');
            // Refresh transactions list after successful categorization
            fetchTransactions();
        }
    });
}
