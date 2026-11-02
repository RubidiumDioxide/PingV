package com.example.wishlist.shared.dto;

import java.util.UUID;

public record CreateWishlistItemRequest(
    UUID creatorId,
    String title,
    String description
) {}
