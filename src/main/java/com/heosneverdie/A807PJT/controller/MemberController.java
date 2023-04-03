package com.heosneverdie.A807PJT.controller;

import com.heosneverdie.A807PJT.common.dto.ResponseDTO;
import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.service.MemberService;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping({"/api/member/"})
@RequiredArgsConstructor
public class MemberController {

    private ResponseDTO responseDTO;
    private static final Logger LOGGER = LoggerFactory.getLogger(MemberController.class);
    private final MemberService memberService;

    // 1. 회원가입 api
    @PostMapping({"signUp"})
    public ResponseEntity<ResponseDTO> SignUp(@RequestBody RequestSignUpDto requestSignUpDto) {
        memberService.signUp(requestSignUpDto);
        responseDTO = new ResponseDTO("정상적으로 가입되었습니다!","",HttpStatus.OK,null);
        return new ResponseEntity<>(responseDTO, HttpStatus.OK);
    }

    // 2. 닉네임 중복검사 api
    @GetMapping({"duplicate/{nickname}"})
    public ResponseEntity<ResponseDTO> duplicateCheck(@PathVariable String nickname) {
        System.out.println(nickname);
        memberService.duplicateNickname(nickname);
        responseDTO = new ResponseDTO("가입 가능한 닉네임입니다.","",HttpStatus.OK,null);
        return new ResponseEntity<>(responseDTO, HttpStatus.OK);
    }

    // 3. 유저의 정보를 가져오는 api
    @GetMapping({"info"})
    public ResponseEntity<ResponseDTO> getUserInfo(@RequestParam String nickname) {
        memberService.getUserInfo(nickname);
        return new ResponseEntity<>(responseDTO, HttpStatus.OK);
    }


}

