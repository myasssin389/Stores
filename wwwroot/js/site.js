// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var toggler = document.getElementsByClassName("caret");
var i;

for (i = 0; i < toggler.length; i++) {
    toggler[i].addEventListener("click", function() {
        this.parentElement.querySelector(".nested").classList.toggle("active");
        this.classList.toggle("caret-down");
    });
}

document.addEventListener('DOMContentLoaded', function() {
    const categoryTabs = document.querySelectorAll('.category-tab');
    const categoryStores = document.querySelectorAll('.category-stores');

    // Show all categories by default
    showAllCategories();

    categoryTabs.forEach(tab => {
        tab.addEventListener('click', function(e) {
            e.preventDefault();

            // Remove active class from all tabs
            categoryTabs.forEach(t => t.classList.remove('active'));

            // Add active class to clicked tab
            this.classList.add('active');

            const selectedCategory = this.getAttribute('data-category');

            if (selectedCategory === 'all') {
                showAllCategories();
            } else {
                showSpecificCategory(selectedCategory);
            }
        });
    });

    function showAllCategories() {
        categoryStores.forEach(store => {
            store.classList.add('active');
        });
    }

    function showSpecificCategory(categoryId) {
        categoryStores.forEach(store => {
            if (store.getAttribute('data-category-id') === categoryId) {
                store.classList.add('active');
            } else {
                store.classList.remove('active');
            }
        });
    }
});

document.querySelector('.shop-cart-toggler').addEventListener('click', function() {
    const cartDropdown = document.getElementById('navbarShopCartDropdown');
    cartDropdown.classList.toggle('show');
});

function checkMax(input, max) {
    if (input > max) {
        alert(`Maximum available quantity is ${max}`);
        input.value = max;
    }
}

async function addToCart(productId) {
    const quantity = parseInt(document.getElementById(`quantity-${productId}`).value) || 1;
    
    const response = await fetch('api/cart/add', {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({productId, quantity})
    })
    
    if (response.ok) {
        const data = await response.json();
        alert(`Added! Total items: ${data.totalItems}, Total: EGP ${data.totalAmount.toFixed(2)}`);
    } else {
        alert ('Failed to add item to cart')
    }
}