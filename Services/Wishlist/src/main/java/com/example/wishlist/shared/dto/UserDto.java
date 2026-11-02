package com.example.wishlist.shared.dto;

import java.util.UUID;

public record UserDto (
    UUID id,
    String login
) {}
