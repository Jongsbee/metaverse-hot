package com.heosneverdie.A807PJT.service;

import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;

public interface MemberService {
    void signUp(RequestSignUpDto requestSignUpDto);

    void duplicateNickname(String nickname);

    void deleteMember();

    void getUserInfo(String nickname);
}
