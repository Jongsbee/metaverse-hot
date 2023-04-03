package com.heosneverdie.A807PJT;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.data.jpa.repository.config.EnableJpaAuditing;

@SpringBootApplication
@EnableJpaAuditing
public class A807PjtApplication {

	public static void main(String[] args) {
		SpringApplication.run(A807PjtApplication.class, args);
	}

}
