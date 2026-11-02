package com.example.wishlist.shared.dto;

public record UpdateWishlistItemRequest(
    String title,
    String description
) {}
