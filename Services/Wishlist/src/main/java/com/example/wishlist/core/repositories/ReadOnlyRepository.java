package com.example.wishlist.core.repositories;

import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.data.repository.Repository;

import java.util.List;
import java.util.Optional;

@NoRepositoryBean
public interface ReadOnlyRepository<T, UUID> extends Repository<T, UUID> {
    Optional<T> findById(UUID id);
    List<T> findAll();
    boolean existsById(UUID id);
    long count();
}
