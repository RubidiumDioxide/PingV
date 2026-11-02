package com.example.wishlist.core.repositories;

import com.example.wishlist.shared.models.*;
import org.springframework.data.repository.CrudRepository;
import java.util.UUID;

public interface WishlistItemRepository extends CrudRepository<WishlistItem, UUID> {
}
