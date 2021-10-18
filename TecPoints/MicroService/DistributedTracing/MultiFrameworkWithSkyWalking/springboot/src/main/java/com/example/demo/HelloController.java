package com.example.demo;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

@RestController
public class HelloController {

	@GetMapping("/")
	public String index() {
		//return "hello world";
		return "springboot respose:\n '" + new RestTemplate().getForObject("http://127.0.0.1:9082", String.class)+'\'';
	}

}
