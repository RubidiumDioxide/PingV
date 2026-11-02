package com.example.wishlist.core.services;

import com.example.wishlist.core.repositories.*;
import com.example.wishlist.core.services.interfaces.WishlistItemService;
import com.example.wishlist.shared.dto.*;
import com.example.wishlist.shared.models.*;
import jakarta.transaction.Transactional;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.UUID;

@Service
public class WishlistItemServiceImpl implements WishlistItemService {
    private final WishlistItemRepository wishlistItemRepository;
    private final UserRepository userRepository;

    public WishlistItemServiceImpl(WishlistItemRepository wishlistItemRepository, UserRepository userRepository) {
        this.wishlistItemRepository = wishlistItemRepository;
        this.userRepository = userRepository;
    }

    @Transactional
    public WishlistItemDto getById(UUID id) {
        WishlistItem wi = wishlistItemRepository.findById(id)
                .orElseThrow(() -> new NoSuchElementException("Wishlist item not found: " + id));

        return toDto(wi);
    }

    @Transactional
    public List<WishlistItemDto> get() {
        return ((List<WishlistItem>) wishlistItemRepository.findAll())
                .stream()
                .map(this::toDto)
                .toList();
    }

    @Transactional
    public WishlistItemDto create(CreateWishlistItemRequest request) {
        User u = userRepository.findById(request.creatorId())
                .orElseThrow(() -> new NoSuchElementException("User not found: " + request.creatorId()));

        WishlistItem wi = new WishlistItem();
        wi.setCreator(u);
        wi.setTitle(request.title());
        wi.setDescription(request.description());

        WishlistItem saved = wishlistItemRepository.save(wi);

        return toDto(saved);
    }

    @Transactional
    public WishlistItemDto update(UUID id, UpdateWishlistItemRequest request) {
        WishlistItem wi = wishlistItemRepository.findById(id)
                .orElseThrow(() -> new NoSuchElementException("WishlistItem not found: " + id));

        wi.setTitle(request.title());
        wi.setDescription(request.description());

        WishlistItem saved = wishlistItemRepository.save(wi);

        return toDto(saved);
    }

    @Transactional
    public void delete(UUID id) {
        if(wishlistItemRepository.existsById(id)){
            wishlistItemRepository.deleteById(id);
        }
    }

    private WishlistItemDto toDto(WishlistItem wi) {
        return new WishlistItemDto(
                wi.getId(),
                wi.getCreator().getId(),
                wi.getCreator().getLogin(),
                wi.getTitle(),
                wi.getDescription()
        );
    }
}