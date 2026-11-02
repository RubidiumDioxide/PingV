package com.example.wishlist.api.controllers;

import com.example.wishlist.core.services.interfaces.WishlistItemService;
import com.example.wishlist.shared.dto.*;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

@Tag(name = "Wishlist")
@RestController
@RequestMapping("/api/v1/wishlist")
public class WishlistController {
    private final WishlistItemService wishlistItemService;

    public WishlistController(WishlistItemService wishlistItemService) {
        this.wishlistItemService = wishlistItemService;
    }

    @GetMapping("/{id}")
    public ResponseEntity<WishlistItemDto> getById(@PathVariable UUID id) {
        return ResponseEntity.ok(wishlistItemService.getById(id));
    }

    @GetMapping()
    public ResponseEntity<List<WishlistItemDto>> get() {
        return ResponseEntity.ok(wishlistItemService.get());
    }

    @PostMapping
    public ResponseEntity<WishlistItemDto> create(@RequestBody CreateWishlistItemRequest request) {
        return ResponseEntity.status(HttpStatus.CREATED).body(wishlistItemService.create(request));
    }

    @PutMapping("/{id}")
    public ResponseEntity<WishlistItemDto> update(
            @PathVariable UUID id,
            @RequestBody UpdateWishlistItemRequest request) {
        return ResponseEntity.ok(wishlistItemService.update(id, request));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable UUID id) {
        wishlistItemService.delete(id);
        return ResponseEntity.noContent().build();
    }
}

