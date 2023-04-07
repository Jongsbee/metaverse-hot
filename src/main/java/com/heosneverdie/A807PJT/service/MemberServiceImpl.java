package com.heosneverdie.A807PJT.service;

import com.heosneverdie.A807PJT.common.exception.member.MemberException;
import com.heosneverdie.A807PJT.common.exception.member.MemberExceptionType;
import com.heosneverdie.A807PJT.data.dto.request.RequestCouponDto;
import com.heosneverdie.A807PJT.data.dto.request.RequestExpCoinDto;
import com.heosneverdie.A807PJT.data.dto.request.RequestSignUpDto;
import com.heosneverdie.A807PJT.data.dto.response.InfoResponseDto;
import com.heosneverdie.A807PJT.data.entity.member.Account;
import com.heosneverdie.A807PJT.data.entity.member.Classes;
import com.heosneverdie.A807PJT.data.entity.member.Coupon;
import com.heosneverdie.A807PJT.data.entity.member.Member;
import com.heosneverdie.A807PJT.data.repository.AccountRepository;
import com.heosneverdie.A807PJT.data.repository.ClassesRepository;
import com.heosneverdie.A807PJT.data.repository.CouponRepository;
import com.heosneverdie.A807PJT.data.repository.MemberRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.Optional;

@Service
@Transactional
@RequiredArgsConstructor
public class MemberServiceImpl implements MemberService {
    private final MemberRepository memberRepository;
    private final AccountRepository accountRepository ;
    private final ClassesRepository classesRepository;
    private final CouponRepository couponRepository;
    private Member member;
    private Coupon coupon;
    private Account account;
    private Classes classes;

    public void signUp(RequestSignUpDto requestSignupDto) {
         member = Member.builder()
                .firebaseId(requestSignupDto.getFirebaseId())
                .nickname(requestSignupDto.getNickname())
                .build();

        member = memberRepository.save(member);

         account = Account.builder()
                .coin(0)
                .exp(0)
                .member(member)
                .build();
        accountRepository.save(account);

         classes = Classes.builder()
                .isWarriorUnlocked(true)
                .isArcherUnlocked(false)
                .isHammerUnlocked(false)
                .isPoorUnlocked(false)
                .member(member)
                .build();

        classesRepository.save(classes);

        coupon = Coupon.builder()
                .member(member)
                .isCoupon1(false)
                .isCoupon2(false)
                .isCoupon3(false)
                .isCoupon4(false)
                .isCoupon5(false)
                .build();

        couponRepository.save(coupon);

        member.updateClasses(classes);
        member.updateAccount(account);
        member.updateCoupon(coupon);
        memberRepository.save(member);
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
    public InfoResponseDto getUserInfo(String nickname) {
        member = memberRepository.findByNickname(nickname).orElseThrow(
                () -> new MemberException(MemberExceptionType.NOT_FOUND_MEMBER));

        // 유저의 정보 가져오기
        InfoResponseDto infoResponseDto = InfoResponseDto.builder()
                .nickname(nickname)
                .coin(member.getAccount().getCoin())
                .exp(member.getAccount().getExp())
                .isHammerUnlocked(member.getClasses().getIsHammerUnlocked())
                .isDeveloperUnlocked(member.getClasses().getIsPoorUnlocked())
                .isAlyakOK(member.getCoupon().getIsCoupon2())
                .build();
        return infoResponseDto;
    }
    @Override
    public void getCouponReward(RequestCouponDto requestCouponDto) {
        // 1. find member
         member = memberRepository.findByNickname(requestCouponDto.getNickname()).orElseThrow(
                () -> new MemberException(MemberExceptionType.NOT_FOUND_MEMBER));

        Optional<Coupon> couponOptional = couponRepository.findByMemberId(member.getId());
        if(couponOptional.isEmpty() || couponOptional == null) {
            System.out.println("여긴오는디?");

            coupon = Coupon.builder()
                    .member(member)
                    .isCoupon1(false)
                    .isCoupon2(false)
                    .isCoupon3(false)
                    .isCoupon4(false)
                    .isCoupon5(false)
                    .build();

            member.updateCoupon(coupon);
            couponRepository.save(coupon);
            memberRepository.save(member);
        }else {
            System.out.println("여길온다구?");
            coupon = couponOptional.get();
            member.updateCoupon(coupon);
            couponRepository.save(coupon);
            memberRepository.save(member);
        }

        String couponString = requestCouponDto.getCouponString().replaceAll(" ", "");
        System.out.println("Coupon string: '" + couponString + "'");

        // 2.
        switch (requestCouponDto.getCouponString()) {
            case "포톤의신해석" :
                if(member.getCoupon().getIsCoupon1()) {
                    System.out.println("이미 등록된 쿠폰");
                    throw new MemberException(MemberExceptionType.ALREADY_USED_COUPON);
                }else {
                    System.out.println("신캐릭 해금!");

                    member.getCoupon().updateCoupon1();
                    member.getClasses().updateHammer();
                    // 캐릭터 해금
                    memberRepository.save(member);
                }
                break;
            case "알약은약차차":
                if(member.getCoupon().getIsCoupon2()) {
                    System.out.println("이미 등록된 쿠폰");
                    throw new MemberException(MemberExceptionType.ALREADY_USED_COUPON);
                }else {
                    System.out.println("알약이 의료보험 완료");
                    // 알약이 버프
                    member.getCoupon().updateCoupon2();
                    memberRepository.save(member);
                    }
                break;
            default :
                throw new MemberException(MemberExceptionType.INVALID_COUPON);
        }
    }

    @Override
    public void updateExpCoin(RequestExpCoinDto requestExpCoinDto) {
        member = memberRepository.findByNickname(requestExpCoinDto.getNickname()).orElseThrow(
                () -> new MemberException(MemberExceptionType.NOT_FOUND_MEMBER));
        member.getAccount().updateExpAndCoin(requestExpCoinDto.getExp(), requestExpCoinDto.getCoin());

        memberRepository.save(member);
    }
}
