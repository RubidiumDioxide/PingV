FROM maven:3.9-eclipse-temurin-21 AS builder

WORKDIR /build

COPY ["Services/Wishlist/pom.xml", "./pom.xml"]
RUN mvn dependency:go-offline -B

COPY ["Services/Wishlist/src", "./src"]
RUN mvn clean package -DskipTests -B

FROM eclipse-temurin:21-jre-alpine
RUN apk add --no-cache wget
WORKDIR /app

COPY --from=builder /build/target/*.jar app.jar

ENTRYPOINT ["java","-jar","/app/app.jar"]