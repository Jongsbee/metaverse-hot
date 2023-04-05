package com.heosneverdie.A807PJT.service;

import com.heosneverdie.A807PJT.common.exception.member.MemberException;
import com.heosneverdie.A807PJT.common.exception.member.MemberExceptionType;
import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.data.entity.member.Account;
import com.heosneverdie.A807PJT.data.entity.member.Classes;
import com.heosneverdie.A807PJT.data.entity.member.Member;
import com.heosneverdie.A807PJT.data.repository.AccountRepository;
import com.heosneverdie.A807PJT.data.repository.ClassesRepository;
import com.heosneverdie.A807PJT.data.repository.MemberRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional
@RequiredArgsConstructor
public class MemberServiceImpl implements MemberService {
    private final MemberRepository memberRepository;
    private final AccountRepository accountRepository ;
    private final ClassesRepository classesRepository;

    public void signUp(RequestSignUpDto requestSignupDto) {
        Member member = Member.builder()
                .firebaseId(requestSignupDto.getFirebaseId())
                .nickname(requestSignupDto.getNickname())
                .build();

        member = memberRepository.save(member);

        Account account = Account.builder()
                .coin(0)
                .exp(0)
                .member(member)
                .build();
        accountRepository.save(account);

        Classes classes = Classes.builder()
                .isWarriorUnlocked(true)
                .isArcherUnlocked(false)
                .isHammerUnlocked(false)
                .isPoorUnlocked(false)
                .member(member)
                .build();

        classesRepository.save(classes);

        member.updateAccount(account);
        member.updateClasses(classes);
    }

    @Override
    public void duplicateNickname(String nickname) {
        if(memberRepository.findByNickname(nickname).isPresent()) { // 닉네임이 있다면
            throw new MemberException(MemberExceptionType.ALREADY_EXIST_NICKNAME);
        }else { // 닉네임이 없다면
            return;
        }
    }

    @Override
    public void deleteMember() {
    }

    @Override
    public void getUserInfo(String nickname) {
        // 유저의 정보 가져오기
    }
}
