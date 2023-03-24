package com.heosneverdie.A807PJT.controller;

import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.service.MemberService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping({"/api"})
public class MemberController {
    private static final Logger LOGGER = LoggerFactory.getLogger(MemberController.class);
    private final MemberService memberService;

    @PostMapping({"/member"})
    public ResponseEntity<?> SignUp(@RequestBody RequestSignUpDto requestSignUpDto) {
        this.memberService.signup(requestSignUpDto);
        return ResponseEntity.status(HttpStatus.OK).build();
    }

    public MemberController(final MemberService memberService) {
        this.memberService = memberService;
    }
}

