package com.example.wishlist.shared.dto;

import java.util.UUID;

public record WishlistItemDto(
    UUID id,
    UUID creatorId,
    String creatorLogin,
    String title,
    String description
) {}
