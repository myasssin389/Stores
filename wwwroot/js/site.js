// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

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

const cartToggler = document.querySelector('.shop-cart-toggler');
if (cartToggler) {
    cartToggler.addEventListener('click', function() {
        const cartDropdown = document.getElementById('navbarShopCartDropdown');
        cartDropdown.classList.toggle('show');
    });
}

function checkMax(input, max) {
    if (input > max) {
        alert(`Maximum available quantity is ${max}`);
        input.value = max;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const tabs = document.querySelectorAll('.option-tab');
    const orders = document.querySelectorAll('.order-card');

    // Define status groups
    const statusGroups = {
        all: ["Pending", "Confirmed", "Shipped", "Delivered", "Cancelled"],
        delivered: ["Delivered"],
        pending: ["Pending"],
        shipped: ["Shipped"],
        cancelled: ["Cancelled"],
        inprogress: ["Pending", "Confirmed", "Shipped"] // grouped under one tab
    };

    // Show all orders by default
    showOrdersByStatuses(statusGroups.all);

    tabs.forEach(tab => {
        tab.addEventListener('click', function (e) {
            e.preventDefault();

            // Remove active from all tabs
            tabs.forEach(t => t.classList.remove('active'));

            // Add active to clicked tab
            this.classList.add('active');

            const selectedKey = this.getAttribute('data-status').toLowerCase();

            if (statusGroups[selectedKey]) {
                showOrdersByStatuses(statusGroups[selectedKey]);
            } else {
                console.warn("No group found for:", selectedKey);
            }
        });
    });

    function showOrdersByStatuses(statusList) {
        orders.forEach(order => {
            const orderStatus = order.getAttribute('data-status');
            if (statusList.includes(orderStatus)) {
                order.style.display = 'block';
            } else {
                order.style.display = 'none';
            }
        });
    }
});