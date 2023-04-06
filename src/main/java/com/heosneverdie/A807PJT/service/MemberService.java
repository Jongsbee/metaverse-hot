package com.heosneverdie.A807PJT.service;

import com.heosneverdie.A807PJT.data.dto.request.RequestCouponDto;
import com.heosneverdie.A807PJT.data.dto.request.RequestExpCoinDto;
import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.data.dto.response.InfoResponseDto;

public interface MemberService {
    void signUp(RequestSignUpDto requestSignUpDto);

    void duplicateNickname(String nickname);

    InfoResponseDto getUserInfo(String nickname);

    void getCouponReward(RequestCouponDto requestCouponDto);

    void updateExpCoin(RequestExpCoinDto requestExpCoinDto);
}
