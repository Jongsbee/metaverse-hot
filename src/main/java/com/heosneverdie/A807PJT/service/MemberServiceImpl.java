package com.heosneverdie.A807PJT.service;

import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.data.entity.member.Member;
import com.heosneverdie.A807PJT.data.repository.MemberRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
public class MemberServiceImpl implements MemberService {
    private final MemberRepository memberRepository;

    public void signup(RequestSignUpDto requestSignupDto) {
        Member.builder().id(requestSignupDto.getId()).nickname(requestSignupDto.getNickname()).build();
    }

    public MemberServiceImpl(final MemberRepository memberRepository) {
        this.memberRepository = memberRepository;
    }
}
