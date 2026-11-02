package com.example.wishlist.core.services.interfaces;

import com.example.wishlist.shared.dto.*;
import java.util.UUID;
import java.util.List;

public interface WishlistItemService {
        WishlistItemDto getById(UUID id);

        List<WishlistItemDto> get();

        WishlistItemDto create(CreateWishlistItemRequest request);

        WishlistItemDto update(UUID id, UpdateWishlistItemRequest request);

        void delete(UUID id);
}
