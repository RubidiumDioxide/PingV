package com.example.wishlist.api;

import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springdoc.core.customizers.OpenApiCustomizer;

import java.util.ArrayList;

@Configuration
public class OpenAPIConfig {
    @Bean
    public OpenAPI wishlistOpenAPI() {
        return new OpenAPI()
                .info(new Info()
                        .title("Wishlist")
                        .version("1.0"));
    }

    @Bean
    public OpenApiCustomizer hideServers() {
        return openApi -> {
            openApi.setServers(new ArrayList<>());
        };
    }
}
