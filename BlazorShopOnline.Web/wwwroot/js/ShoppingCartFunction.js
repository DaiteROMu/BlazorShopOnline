function makeUpdateQuantityBtnVisible(id, visible) {
    const updateQuantityButton = document.querySelector(`button[data-itemid="${id}"]`);

    console.log(updateQuantityButton);

    if (visible) {
        updateQuantityButton.classList.remove('update-quantity-btn__hidden');
    } else {
        updateQuantityButton.classList.add('update-quantity-btn__hidden');
    }
}